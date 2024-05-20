import { useQuery } from "@tanstack/react-query";
import { Outlet, createLazyFileRoute } from "@tanstack/react-router";
import { getApplications } from "../services/auth";

export const Route = createLazyFileRoute("/admin")({
    component: Admin,
});

function Admin() {
    const { data } = useQuery({
        queryKey: ["test"],
        queryFn: getApplications,
    });

    return (
        <div className="p-2">
            <div>{data}</div>
            <Outlet />
        </div>
    );
}
