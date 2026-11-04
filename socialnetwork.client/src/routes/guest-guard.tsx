import { Navigate, Outlet } from "react-router-dom";
import { useAppState } from "../pages/useAppStore";

export function GuestGuard() {
    const { user, session } = useAppState();

    const isAuthenticated =
        user !== null &&
        session?.accessToken != null;

    return isAuthenticated ? <Navigate to="/" replace /> : <Outlet />;
}
