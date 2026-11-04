import { BrowserRouter, Route, Routes } from "react-router-dom";
import { RoleGuard } from "./routes/RoleGuard";
import RegisterPage from "./pages/auth/pages/register-page/register-page";
import LoginPage from "./pages/auth/pages/login-page/login-page";
import HomePage from "./pages/home/home-page";
import { GuestGuard } from "./routes/guest-guard";

export default function App() {
    return (
        <BrowserRouter>
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
                    <Route
                        path="/"
                        element={<HomePage />}
                    />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}