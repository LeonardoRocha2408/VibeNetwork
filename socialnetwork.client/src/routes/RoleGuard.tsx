import { useAppState } from "../pages/useAppStore";
import { Navigate, Outlet, useLocation } from "react-router-dom";

interface RoleGuardProps {
    allowedRoles: ("User" | "Admin")[];
}

export function RoleGuard({allowedRoles}: RoleGuardProps) {
    const {user, session} = useAppState();;
    const location = useLocation();

    if (!session?.accessToken || !user || session.expireAt < new Date()) {
        return <Navigate to="/login" state={{from: location}} replace/>
    }
    if (allowedRoles && !allowedRoles.includes(user.role)) {
        return <Navigate to="/forbbiden"/>
    }

    return <Outlet />
}
