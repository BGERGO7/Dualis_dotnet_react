import { useNavigate } from "react-router";
import { menuItems } from "./menuItems";

export function Menu() {
  const navigete = useNavigate();

  return (
    <>
      {menuItems.map((mi, index) => {
        return (
          <div
            key={index}
            style={{ cursor: "pointer" }}
            onClick={() => {
              navigete(mi.url);
            }}
          >
            {mi.label}
          </div>
        );
      })}
    </>
  );
}
