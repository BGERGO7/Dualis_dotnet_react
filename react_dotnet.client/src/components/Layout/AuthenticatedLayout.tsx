import { use } from "react";
import { useAccessToken } from "../../hooks/useAccessToken";
import { LoginForm } from "../LoginForm/LoginForm";
import { Outlet } from "react-router";

export function AuthenticatedLayout() {
  const { accessToken } = useAccessToken();

  return <>{accessToken ? <Outlet /> : <LoginForm />}</>;
}
