// Sends a calendar date without a time zone ("1990-01-01T00:00:00"); Date.toJSON() would
// convert it to UTC and, east of Greenwich, shift the birthday to the previous day.
export function toLocalDate(value: Date | string) {
  if (!(value instanceof Date)) {
    return value;
  }
  const pad = (n: number) => String(n).padStart(2, '0');
  return `${value.getFullYear()}-${pad(value.getMonth() + 1)}-${pad(value.getDate())}T00:00:00`;
}
