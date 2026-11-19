import { BrowserRouter, Route, Routes } from "react-router-dom";
import { RoleGuard } from "./routes/RoleGuard";
import RegisterPage from "./pages/auth/pages/register-page/register-page";
import LoginPage from "./pages/auth/pages/login-page/login-page";
import HomePage from "./pages/home/home-page";
import { GuestGuard } from "./routes/GuestGuard";
import { useAppState } from "./pages/useAppStore";
import ProtectedLayout from "./routes/ProtectedLayout";

export default function App() {
    const { theme } = useAppState();
    return (
        <BrowserRouter>
            <div className={`min-h-dvh ${theme === "dark" ? "theme-dark app-background" : "theme-light app-background"}`}>
                <Routes>
                    <Route element={<GuestGuard />} >
                        <Route
                            path="/register_account"
                            element={<RegisterPage />}
                        />
                        <Route
                            path="/login"
                            element={<LoginPage />}
                        />
                    </Route>
                    
                    <Route element={<RoleGuard allowedRoles={["User", "Admin"]} />}>
                        <Route element={<ProtectedLayout />} >
                            <Route
                                path="/"
                                element={<HomePage />}
                            />
                        </Route>
                    </Route>
                </Routes>
            </div>
        </BrowserRouter>
    );
}