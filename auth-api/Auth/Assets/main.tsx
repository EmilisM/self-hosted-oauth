import "vite/modulepreload-polyfill";
import { createRoot } from "react-dom/client";
import App from "./App";
import "./index.css";

const appElement = document.querySelector<HTMLDivElement>("#app")!;

const root = createRoot(appElement);
root.render(<App />);
