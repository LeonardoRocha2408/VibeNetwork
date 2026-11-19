import { NavLink } from "react-router-dom";
import { useAppState } from "../pages/useAppStore";
import { Home, Search, Bell, User, Icon } from "lucide-react";

const links = [
    { to: "/", label: "Home", icon: Home },
    { to: "/search", label: "Search", icon: Search },
    { to: "/notifications", label: "Notifications", icon: Bell },
    { to: "/profile", label: "Profile", icon: User }
];

export default function Navbar() {
    const { user } = useAppState();

    return (
        <div className="flex md:bg-[#140F35] justify-center p-1
        md:items-center md:fixed md:top-0 md:left-0 md:flex md:flex-col md:bg-black/30 md:min-h-dvh">
            {links.map(({to, label, icon: Icon}) => (
                <NavLink 
                    key={label}
                    to={to}
                    className={({isActive}) => `flex flex-1 md:w-16 md:pr-2 md:items-center flex-col justify-center items-center sl:border-b-4 md:border-r-4
                    ${isActive ? "border-[#7e22ce] text-white" : "text-[#70698D] border-transparent"}`}
                > 
                    <Icon size={30} />
                    <span className="text-[15px] md:text-[11px]">{label}</span>
                </NavLink>
            ))}
        </div>
    )
}