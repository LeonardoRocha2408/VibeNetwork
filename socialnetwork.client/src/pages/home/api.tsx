import { useAppState } from "../useAppStore"

interface LikePost {
    postId: string,
    userId: string | undefined,
    isLiked: boolean,
    accessToken: string | null | undefined
}

export async function likePost({postId, userId, isLiked, accessToken}: LikePost) {
    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/feed/like-post`, {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`
            },
            body: JSON.stringify({
                PostId: postId,
                UserId: userId,
                IsLiked: isLiked
            })
        })
        if (!response.ok)
            throw new Error("Invalid response from server")
        return true;
    }
    catch (error) {
        console.log(error);
    }
}