import { parse, z } from "zod";
import { sessionSchema, userSchema } from "../useAppStore";

const authResponseSchema = z.object({
    session: sessionSchema,
    user: userSchema
});
const API_URL = import.meta.env.VITE_API_URL;

export async function register(email: string, username: string, password: string) {
    const response = await fetch(`${API_URL}/api/users/create-user`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({email, username, password})
    })
    if (!response.ok) {
        throw new Error("Failed to register user");
    }
}

export async function login(email: string, password: string) {
    const response = await fetch(`${API_URL}/api/users/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({email, password})
    }) 
    if (!response.ok) {
        throw new Error("Failed to login user");
    }
    const data = await response.json();
    const parsed = authResponseSchema.parse(data);
    
    if (!parsed.session || !parsed.user) {
        throw new Error("Invalid response from server");
    }

    return {
        session: parsed.session,
        user: parsed.user,
    }
}
