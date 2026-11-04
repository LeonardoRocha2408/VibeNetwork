export default function CornerGlow() {
    return (
        <>
            <div className="pointer-events-none fixed -left-36 -top-38 z-0 size-80 
            rounded-full bg-gradient-to-br from-violet-700/80 via-blue-700/50 to-transparent blur-3xl" />
            
            <div className="pointer-events-none fixed -left-3 -top-70 z-0 size-80 
            rounded-full bg-gradient-to-br from-violet-700/80 via-blue-700/50 to-transparent blur-3xl" />

            <div className="pointer-events-none fixed -bottom-40 -right-36 z-0 size-96 
            rounded-full bg-gradient-to-tl from-violet-700/70 via-blue-700/40 to-transparent blur-3xl" />

            <div className="pointer-events-none fixed -right-90 z-0 size-96 
            rounded-full bg-gradient-to-tl from-violet-700/70 via-blue-700/40 to-transparent blur-3xl" />

            <div className="pointer-events-none fixed -bottom-90 -left-30 z-0 size-100
            rounded-full bg-gradient-to-tl from-violet-700/70 via-blue-700/40 to-transparent blur-3xl" />
        </>
    );
}