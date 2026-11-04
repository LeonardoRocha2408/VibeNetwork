import { Mail, Lock } from "lucide-react";
import { Link, useNavigate } from "react-router-dom";
import AuthPage from "../auth-layout";
import { loginSchema } from "./login-schema";
import { login } from "../../api";
import { useAppState } from "../../../useAppStore";

const inputs = [
    { type: "email", label: "E-mail", id: "email", icon: Mail },
    { type: "password", label: "Password", id: "password", icon: Lock}
]

export default function LoginPage() {
    const navigate = useNavigate();
    const saveLogin = useAppState((state) => state.login);
    return (
    <AuthPage 
    fields={inputs}
    schema={loginSchema}
    submitLabel="Login account"
    onSubmit={async (v) => {
        const result = await login(v.email, v.password);
        saveLogin(result.session, result.user);
        navigate("/");
    }}
    title={
        <>
        Login{" "}
        <span className="bg-linear-to-r from-[#7e22ce] to-[#1d4ed8] bg-clip-text text-transparent">
        account
        </span>
        </>
    }
    accountExists={
        <>
        Don't have an account yet?{" "}
        <Link to="/register_account">Sign up</Link>
        </>
    }
    />
    )
}