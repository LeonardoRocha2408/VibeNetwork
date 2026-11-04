import { LucideIcon,  Eye, EyeOff } from "lucide-react";
import { useState, type ReactNode } from "react";
import { z } from "zod";
import Logo from "../../../../images/logo.png"
import CornerGlow from "../../../components/CornerGlow";

interface Field {
  type: string;
  label: string;
  id: string;
  icon: LucideIcon
}

type Values = Record<string, string>;

interface AuthPageProps {
  fields: readonly Field[];
  schema: z.ZodType;
  onSubmit: (values: Values) => Promise<void>;
  canSubmit?: (values: Values) => boolean;
  extra?: (values: Values) => ReactNode;
  submitLabel: string;
  title: ReactNode;
  accountExists: ReactNode;
}

export default function AuthPage({fields, schema, onSubmit, canSubmit, extra, submitLabel, title, accountExists}: AuthPageProps) {
  const [passwordVisible, setPasswordVisible] = useState<Record<string, boolean>>({});
  const [form, setForm] = useState<Values>(() =>
    Object.fromEntries(fields.map((f) => [f.id, ""]))
  );
  const [error, setError] = useState<Record<string, string[] | undefined>>({});
  const [serverError, setServerError] = useState("");

  const isValid = canSubmit ? canSubmit(form) : true;

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
    e.preventDefault();
    setServerError("");

    const result = schema.safeParse(form);

    if (!result.success) {
      setError(z.flattenError(result.error).fieldErrors);
      return;
    }
    setError({});

    try {
      await onSubmit(form);
    } catch (err) {
      setServerError((err as { title?: string }).title ?? "Something went wrong.");
    }
  }

  return (
    <div className="flex flex-col items-center justify-center h-screen w-screen bg-black overflow-hidden">
      <CornerGlow />
      <img src={Logo} className="size-25"/>
      <h1 className="text-2xl text-white font-bold mt-2 mb-1">{title}</h1>
      <p className="text-gray-500 text-[13px]">Type your data accounts to continue</p>

      <form onSubmit={handleSubmit} className="flex items-center w-[85%] max-w-80 flex-col sm:max-w-96">
        {fields.map((input) => {
        const Icon = input.icon;

        return (
            <div key={input.id} className="border-gradient flex w-full items-center rounded-[13px] p-1 py-1.60 m-1">
                <label htmlFor={input.id} className="ml-2.5 mr-1"> 
                    <Icon size={20} color="#7e22ce"/> 
                </label>

                <input
                    className="border-none focus:outline-none text-[13px] text-white p-1 w-full"
                    id={input.id}
                    type={input.type === "password" && passwordVisible[input.id] ? "text" : input.type}
                    value={form[input.id]}
                    name={input.id}
                    placeholder={input.label}
                    onChange={handleChange}
                />
                {input.type === "password" && 
                <button 
                className="mr-2.5"
                type="button"
                onClick={() => setPasswordVisible((previous) => ({
                    ...previous, [input.id]: !previous[input.id]
                }))}>
                    {passwordVisible[input.id] ? <Eye size={15} color="white"/> : <EyeOff size={15} color="white"/>}
                </button>}

                {error[input.id]?.map((msg) => (
                    <span className="text-red-500" key={msg}>{msg}</span>
                ))}
            </div>
        );
        })}

        {extra?.(form)}

        {serverError && <p>{serverError}</p>}

        <button 
          className="w-full bg-linear-to-r from-violet-600 via-blue-600 to-cyan-400 
          rounded-full py-2 font-bold text-amber-50 mt-1
          transition duration-1000 hover:scale-[1.05]"
          type="submit" 
          disabled={!isValid}>
          {submitLabel}
        </button>
      </form>
      <p className="text-gray-500 text-[13px]">{accountExists}</p>
    </div>
  );
}