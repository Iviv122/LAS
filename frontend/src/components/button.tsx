import type React from "react"

interface ButtonProps{
    label: string
    type?: "submit" | "reset"  | "button" | undefined
    onClick?: () => {}
    ref?: React.Ref<HTMLButtonElement> | undefined
    className?: string
}

export default function Button({label,className,type,onClick,ref} : ButtonProps){
    return <button 
    type={type}
    onClick={onClick}
    ref={ref}
    className={`
        p-4
        bg-green-400
        cursor-pointer
        w-fit
        text-2xl
        rounded-xl
        `
         + " " + className}
    >{label}</button>
}