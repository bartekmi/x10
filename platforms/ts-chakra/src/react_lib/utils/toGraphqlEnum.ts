// This function is used to convert enums to the value as needed by GraphQL
// Some GQL servers require all-caps for enum names

export default function toGraphqlEnum(value?: string): string | null {
  if (value == null) {
    return null;
  }

  return value.toUpperCase();
}