import { z } from "zod";

export const registerSchema = z.object({
    email: z.email("Invalid e-mail"),
    username: z
    .string()
    .min(1)
    .max(20)
    .regex(/^[a-zA-Z0-9_]+$/, "Use only letter, numbers and _"),
    
})