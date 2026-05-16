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
        m-2
        shadow-xl
        cursor-pointer
        w-fit
        text-2xl
        rounded-xl
        border-2
        border-stone-100
        `
         + " " + className}
    >{label}</button>
}