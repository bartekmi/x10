// @flow

export const PetPolicyEnumPairs = [
  {
    label: "All Pets OK",
    value: "ALL_PETS_OK",
  },
  {
    label: "Cats Only",
    value: "CATS_ONLY",
  },
  {
    label: "Dogs Only",
    value: "DOGS_ONLY",
  },
  {
    label: "No Pets",
    value: "NO_PETS",
  },
]

export type PetPolicyEnum = "ALL_PETS_OK" |  "CATS_ONLY" |  "DOGS_ONLY" |  "NO_PETS";
