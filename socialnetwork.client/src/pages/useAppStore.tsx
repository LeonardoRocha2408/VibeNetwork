import { z } from "zod";
import { create } from "zustand";
import { persist, createJSONStorage,} from "zustand/middleware";

export const userSchema = z.object({
    id: z.uuid(),
    email: z.email(),
    username: z.string().nullable(),
    displayName: z.string().nullable(),
    profilePictureUrl: z.string(),
    role: z.enum(["User", "Admin"])
});

export const sessionSchema = z.object({
    accessToken: z.string().nullable(),
    expireAt: z.coerce.date()
});

export type User = z.infer<typeof userSchema>;
export type Session = z.infer<typeof sessionSchema>;

interface AppState {
    session: Session | null
    user: User | null
    isExpired: () => boolean
    login: (session: Session, user: User) => void
    logout: () => void
}

export const useAppState = create<AppState>()(
    persist(
        (set, get) => ({
            session: null,
            user: null,

            isExpired: () => {
                const session = get().session;
                return !session || session.expireAt <= new Date();
            },
            login: (session, user) => set({
                session: sessionSchema.parse(session), user: userSchema.parse(user)
            }),

            logout: () => set({session: null, user: null})
        }),
        {
            name: "vibenetwork-storage",
            storage: createJSONStorage(() => localStorage)
        }
    )
)
