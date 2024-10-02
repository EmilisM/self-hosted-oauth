import { useSearch } from "wouter";
import { Button } from "./components/ui/button";

function App() {
  const search = useSearch();
  const urlSearchParams = new URLSearchParams(search);
  const redirectUri = urlSearchParams.get("redirect_uri");

  const mock = () => {
    if (!redirectUri) {
      return;
    }

    fetch("https://localhost:5000/callback/login/mock", {
      credentials: "include",
    }).then(() => {
      window.location.href = redirectUri;
    });
  };

  return (
    <div className="h-dvh flex flex-col justify-center items-center">
      Hi from auth-web!
      <Button onClick={mock}>Mock</Button>
    </div>
  );
}

export default App;
