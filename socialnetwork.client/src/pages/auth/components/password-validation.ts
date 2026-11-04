export function passwordValidation(password: string) {
    const requirements = [
        {
            label: "At least 8 characters",
            valid: password.length >= 8 
        },
        {
            label: "At least 1 uppercase letter",
            valid: /[A-Z]/.test(password)
        },
        {
            label: "At least 1 lowercase letter",
            valid: /[a-z]/.test(password)
        },
        {
            label: "At least 1 number",
            valid: /[0-9]/.test(password)
        },
        {
            label: "At least 1 special character",
            valid: /[^A-Za-z0-9]/.test(password)
        }
    ]
    return requirements;
}

export function isValidPassword(password: string) {
    return passwordValidation(password).every((r) => r.valid);
}