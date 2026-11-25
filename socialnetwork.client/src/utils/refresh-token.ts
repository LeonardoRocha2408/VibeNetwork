import { type User, type Session, sessionSchema } from "../pages/useAppStore"

interface RefreshTokenProps {
    user: User | null,
    session: Session | null
}
export async function refreshToken({user, session}: RefreshTokenProps): Promise<Session | null> {
    if (user === null || session === null)
        return null;
    try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/users/refresh-token`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${session.accessToken}`
                },
                body: JSON.stringify({
                    User: user
                })
            })
            if (!response.ok) 
                return null;

            const data = await response.json();
            const parsed = sessionSchema.safeParse(data);

            if (!parsed.success)
                return null;

            return parsed.data;
        }
        catch (error) {
            alert(error);
            return null;
        }
}