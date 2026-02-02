export interface MenuItem {
  label: string;
  url: string;
}

export interface JwtData {
  firstName: string;
  lastName: string;
  //? azt jelenti hogy lehet undefined is
  ["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]?:
    | string
    | string[];
}

export interface ProductDto {
  id: number;
  name: string;
  description?: string | null;
  price: number;
}
