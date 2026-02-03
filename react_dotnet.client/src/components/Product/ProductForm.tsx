import React, { useState } from "react";
import type { ProductDto } from "../../types";

interface ProductFormProps {
  onSubmit?: (product: ProductDto, reset: () => void) => void;
}

export function ProductForm({ onSubmit }: ProductFormProps) {
  const defaultValues: ProductDto = {
    id: 0,
    name: "",
    description: "",
    price: 0,
  };

  const [product, setProduct] = useState<ProductDto>(defaultValues);

  const onChange =
    (key: keyof ProductDto) =>
    (event: React.ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
      setProduct((prevProduct) => {
        return { ...prevProduct, [key]: event.target.value };
      });
    };

  const reset = () => {
    setProduct({ ...defaultValues });
  };

  return (
    <>
      <div>
        <input
          type="text"
          name="name"
          placeholder="Megnevezés"
          value={product.name}
          onChange={(e) => onChange("name")(e)}
        />
      </div>
      <div>
        <input
          type="number"
          name="price"
          placeholder="Ár"
          value={product.price}
          onChange={(e) => onChange("price")(e)}
        />
      </div>
      <div>
        <input
          type="text"
          name="description"
          placeholder="Leírás"
          value={product.description ?? ""}
          onChange={(e) => onChange("description")(e)}
        />
      </div>

      <div>
        <input
          type="button"
          value="Küldés"
          onClick={() => {
            onSubmit?.(product, reset); //?. ha nem undefined vagy nem null akkor meghívja
          }}
        />
      </div>
    </>
  );
}
