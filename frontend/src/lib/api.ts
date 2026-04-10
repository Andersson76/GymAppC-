const API_URL = process.env.NEXT_PUBLIC_API_URL;

if (!API_URL) {
  throw new Error("NEXT_PUBLIC_API_URL is not defined");
}

export async function apiFetch<T>(
  endpoint: string,
  options?: RequestInit
): Promise<T> {
  const response = await fetch(`${API_URL}${endpoint}`, {
    headers: {
      "Content-Type": "application/json",
      ...(options?.headers || {}),
    },
    ...options,
  });

  const data = await response.json().catch(() => null);
    
  console.log("API_URL:", process.env.NEXT_PUBLIC_API_URL);
  
  if (!response.ok) {
    throw new Error(data?.message || "Något gick fel i API-anropet");
  }

  return data as T;
}