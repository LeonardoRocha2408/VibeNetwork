import Logo from "../../images/logo.png";

export default function Header() {
    return (
        <header className="flex items-center pt-4 pl-4 mb-4">
            <img src={Logo} className="w-15"/>
            <h1 className="text-[30px] bg-linear-to-r from-white via-[#7e22ce] to-[#1d4ed8] 
            bg-clip-text text-transparent font-bold">
                Vibe
            </h1>

        </header>
    )
}