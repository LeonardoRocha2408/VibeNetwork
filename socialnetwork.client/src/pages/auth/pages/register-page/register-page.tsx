import { Mail, Lock, LockKeyhole, User } from "lucide-react";
import { Link, useNavigate } from "react-router-dom";
import AuthPage from "../auth-layout";
import PasswordRequirements from "../../components/password-requeriments";
import { isValidPassword } from "../../components/password-validation";
import { register } from "../../api";
import { registerSchema } from "./register-schemas";

const inputs = [
  { type: "email", label: "E-mail", id: "email", icon: Mail },
  { type: "text", label: "Username", id: "username", icon: User},
  { type: "password", label: "Password", id: "password", icon: Lock },
  { type: "password", label: "Confirm password", id: "confirmPassword", icon: LockKeyhole },
] as const;

export default function RegisterPage() {
  const navigate = useNavigate();
  return (
    <AuthPage
      fields={inputs}
      schema={registerSchema}
      submitLabel="Create account"
      canSubmit={(v) => isValidPassword(v.password) && v.password === v.confirmPassword}
      extra={(v) => <PasswordRequirements password={v.password} />}
      onSubmit={async (v) => {
        await register(v.email, v.username, v.password);
        navigate("/login");
      }}
      title={
          <>
          Create{" "}
          <span className="bg-linear-to-r from-[#7e22ce] to-[#1d4ed8] bg-clip-text text-transparent">
            account
          </span>
          </>
        }
      accountExists={
          <>
          Already have an account yet?{" "}
          <Link to="/login">Sign in</Link> 
          </>
      }
    />
  );
}