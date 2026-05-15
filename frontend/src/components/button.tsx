import type React from "react"

interface ButtonProps{
    label: string
    type: "submit" | "reset"  | "button" | undefined
    onClick: () => {}
    ref: React.Ref<HTMLButtonElement> | undefined
    className: string
}

export default function Button({label,className,type,onClick,ref} : ButtonProps){
    return <button 
    type={type}
    onClick={onClick}
    ref={ref}
    className={"p-1 bg-green-400 " + " " + className}
    >{label}</button>
}