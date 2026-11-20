import { useEffect, useState } from "react";
import { useAppState } from "../useAppStore";
import PostCard from "../../components/posts";

export default function HomePage() {
    const { user, session } = useAppState();
    const accessToken = session?.accessToken;
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(0);

    useEffect(() => {
        const fetchPosts = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_API_URL}/api/feed/get-feed`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${session?.accessToken}`
                    },
                    body: JSON.stringify({
                        UserId: user?.id,
                        FollowedsCount: user?.followedCount,
                        Page: page + 1
                    })
                });
                if (!response.ok) {
                    throw new Error("Failed to fetch posts");
                }
                const data = await response.json();
                setPosts(data);
            }
            catch (error) {
                console.error(error);
            }
            finally {
                setLoading(false);
            }            
        }
        fetchPosts();
    }, [session?.accessToken])

    return (
        <div className="h-screen w-screen overflow-x-hidden">
            <PostCard posts={posts} userId={user?.id} accessToken={session?.accessToken}/>
        </div>
    );
}