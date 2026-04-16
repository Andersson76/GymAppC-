const getApiBaseUrl = () => {
  const apiBaseUrl =
    typeof window === "undefined"
      ? process.env.INTERNAL_API_URL
      : process.env.NEXT_PUBLIC_API_URL;

  if (!apiBaseUrl) {
    throw new Error("API URL is not defined");
  }

  return apiBaseUrl;
};

export async function apiFetch<T>(
  endpoint: string,
  options?: RequestInit
): Promise<T> {
  const apiBaseUrl = getApiBaseUrl();

  const response = await fetch(`${apiBaseUrl}${endpoint}`, {
    headers: {
      "Content-Type": "application/json",
      ...(options?.headers || {}),
    },
    ...options,
  });

  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new Error(data?.message || "Något gick fel i API-anropet");
  }

  return data as T;
}