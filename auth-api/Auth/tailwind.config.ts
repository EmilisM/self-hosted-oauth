import type { Config } from 'tailwindcss'

export default {
    content: [
        "./Assets/**/*.{ts,tsx}",
        "./Pages/**/*.{cshtml,razor}",
    ],
    transform: {
        // Allow the use of the @@ character in Razor files.
        razor: (content: string) => content.replace(/@@/g, "@"),
    },
    theme: {
        extend: {},
    },
    plugins: [],
} satisfies Config;