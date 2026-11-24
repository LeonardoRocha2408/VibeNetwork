import { ReactNode, useEffect } from "react";
import { refreshToken } from "../utils/refresh-token";
import { useAppState } from "../pages/useAppStore";

interface TokenRefreshManagerProps {
    children: ReactNode;
}

export default function TokenRefreshManager({ children }: TokenRefreshManagerProps) {
    const { user, session, login } = useAppState();
    useEffect(() => {
        if (!session || session.expireAt == undefined)
            return;

        console.log("Iniciando a contagem")
        const expirationTime = new Date(session?.expireAt).getTime()
        const timeRemaining = expirationTime - Date.now() - 60_000;

        const timeout = setTimeout(async () => {
            const newSession = await refreshToken({ user, session });

            if (newSession && user !== null) {
                login(newSession, user);
            }
        }, Math.max(0, timeRemaining));

        return () => clearTimeout(timeout);
    }, [user, session])

    return null;
}