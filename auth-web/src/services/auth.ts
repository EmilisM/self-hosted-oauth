import { QueryClient } from "@tanstack/react-query";

export const authClient = new QueryClient();

export async function getApplications() {
    const response = await fetch("/api/applications");

    return response.json();
}
