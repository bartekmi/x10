// @flow

type EnumValueType = {
  label: string,
  value: string,
};

type EnumType = $ReadOnlyArray<EnumValueType>;

export default function enumToLabel(enumArray: EnumType, value: ?string): ?string {
  if (value == null) {
    return null;
  }

  const enumValue = enumArray.find(x => x.value === value);
  if (enumValue == null) {
    throw `Illegal enum value ${value}. Expected one of: ${enumArray.map(x => x.value).join()}`;
  }
  return enumValue.label;
}