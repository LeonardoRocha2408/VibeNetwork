import { useState } from "react";
import { Heart, MessageCircle, Repeat2, Icon } from "lucide-react";
import { likePost } from "../pages/home/api";

interface Post {
    id: string,
    userDisplayName: string,
    profilePictureUrl: string,
    content: string,
    likesCount: number,
    commentsCount: number,
}

interface PostProps {
    posts: Post[],
    userId: string | undefined,
    accessToken: string | null | undefined
}


export default function PostCard({posts, userId, accessToken}: PostProps) {
    const [like, setLike] = useState<Record<string, boolean>>({});
    const [likeCount, setLikeCount] = useState<Record<string, number>>(() => 
            Object.fromEntries(posts.map(post => [post.id, post.likesCount])))
    const [reposted, setReposted] = useState(false)

    return (
        <div className="flex flex-col items-center justify-center select-none">
            {posts.map((post) => (
                <div key={post.id} 
                    className="app-card m-2 px-5 md:px-7 py-5 rounded-2xl w-[90%] md:w-[60%]">
                    <div className="flex items-center gap-1.5 text-[15px]">
                        <img src={`${post.profilePictureUrl || "https://i.pravatar.cc/150?img=12"}`} 
                             className="rounded-[50%] w-[11%] md:w-[4%]"
                        />
                        <span>{post.userDisplayName}</span>
                    </div>
                    <p>
                        {post.content}
                    </p>
                    <div className="flex items-center justify-end-safe select-none gap-2">
                        <div className="flex items-center gap-1">
                            <span>{likeCount[post.id] ?? post.likesCount}</span>
                            <Heart 
                                size={20} 
                                color={like[post.id] ? "red" : "white"} 
                                fill={like[post.id] ? "red" : "none"}
                                onClick={() => {
                                    const isLiked = like[post.id] ?? false;
                                    setLike(prev => ({
                                        ...prev, [post.id]: !isLiked
                                    }));
                                    setLikeCount(prev => ({
                                        ...prev, [post.id]: (prev[post.id] ?? post.likesCount) + (isLiked ? -1 : 1)
                                    }));
                                    likePost({postId: post.id, userId: userId, isLiked: isLiked, accessToken: accessToken});
                                }}/>
                        </div>
                        <div className="flex items-center gap-1">
                            <span>{post.commentsCount}</span>
                            <MessageCircle size={20} color="white"/>
                        </div>
                        <div className="flex items-center gap-1">
                            <span>0</span>
                            <Repeat2 size={20} color="white"/>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    )
}