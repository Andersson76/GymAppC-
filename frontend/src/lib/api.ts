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
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(options?.headers ?? {}),
    },
  });

  const rawText = await response.text();
  let data: unknown = null;

  try {
    data = rawText ? JSON.parse(rawText) : null;
  } catch {
    data = rawText;
  }

   if (!response.ok) {
    let errorMessage = `API error ${response.status}`;

    if (response.status === 403) {
      errorMessage = "Endast admin kan radera träningspass.";
    } else if (
      typeof data === "object" &&
      data !== null &&
      "message" in data &&
      typeof data.message === "string"
    ) {
      errorMessage = data.message;
    } else if (
      typeof data === "object" &&
      data !== null &&
      "title" in data &&
      typeof data.title === "string"
    ) {
      errorMessage = data.title;
    } else if (typeof data === "string" && data.trim() !== "") {
      errorMessage = data;
    }
    throw new Error(errorMessage);
  }

  return data as T;

}

function getAuthToken() {
  if (typeof window === "undefined") return null;
  return localStorage.getItem("token");
}

function getAuthHeaders(): HeadersInit {
  const token = getAuthToken();

  return token
    ? {
        Authorization: `Bearer ${token}`,
      }
    : {};
}

export type Workout = {
  id: number;
  title: string;
  date: string;
  notes?: string;
  userId: number;
};

export type CreateWorkoutDto = {
  title: string;
  date: string;
  notes?: string;
};

export async function getWorkouts(): Promise<Workout[]> {
  return apiFetch<Workout[]>("/api/workouts", {
    method: "GET",
    headers: {
      ...getAuthHeaders(),
    },
  });
}

export async function createWorkout(
  workout: CreateWorkoutDto
): Promise<Workout> {
  return apiFetch<Workout>("/api/workouts", {
    method: "POST",
    headers: getAuthHeaders(),
    body: JSON.stringify(workout),
  });
}

export async function deleteWorkout(id: number): Promise<void> {
  await apiFetch<void>(`/api/workouts/${id}`, {
    method: "DELETE",
    headers: {
      ...getAuthHeaders(),
    },
  });
}

export type CurrentUser = {
  id: string;
  email: string;
  role: string;
};

export async function getCurrentUser(): Promise<CurrentUser> {
  return apiFetch<CurrentUser>("/api/user/me", {
    method: "GET",
    headers: getAuthHeaders(),
  });
}