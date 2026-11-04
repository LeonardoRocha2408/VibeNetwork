import { CircleCheck, Circle, ShieldCheck } from "lucide-react";
import { passwordValidation } from "./password-validation";
interface PasswordRequirementsProps {
    password: string
}

export default function PasswordRequirements({ password }: PasswordRequirementsProps) {
    const requirements = passwordValidation(password);

    return (
        <div className="flex w-full border-2 border-[#23266b] bg-[#0a0b24] rounded-[15px] p-2.5 m-2">
            <ShieldCheck size={40} color="#3b0764"/>
            <div className="ml-3.5">
                <span className="text-[13px] font-bold text-white">Your password has must:</span>
                <ul className="list-none">
                    {requirements.map((requirement) => (
                        <li 
                            className="flex items-center text-gray-400 space-x-1 gap-2 text-[12px]"
                            key={requirement.label}
                        >
                            {requirement.valid ? 
                            <CircleCheck size={10} color="#3b82f6"/> : 
                            <Circle size={10} color="grey"/>} {requirement.label}
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}