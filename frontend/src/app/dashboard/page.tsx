"use client";

import { useEffect, useState } from "react";
import { getWorkouts, createWorkout, deleteWorkout, getCurrentUser, CurrentUser, } from "@/lib/api";
import { Workout } from "@/types/workout";

export default function DashboardPage() {
  const [workouts, setWorkouts] = useState<Workout[]>([]);
  const [title, setTitle] = useState("");
  const [date, setDate] = useState("");
  const [notes, setNotes] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [user, setUser] = useState<CurrentUser | null>(null);

  async function loadWorkouts() {
    try {
      setLoading(true);
      setError("");
      const data = await getWorkouts();
      setWorkouts(data);
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Kunde inte hämta träningspass."
      );
    } finally {
      setLoading(false);
    }
  }

  async function loadCurrentUser() {
  try {
    const currentUser = await getCurrentUser();
    setUser(currentUser);
  } catch (error) {
    setError(
      error instanceof Error
        ? error.message
        : "Kunde inte hämta användarinformation."
    );
  }
}

  useEffect(() => {
    loadWorkouts();
    loadCurrentUser();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    try {
      setError("");
      setSuccessMessage("");

      const formattedDate = date.length === 16 ? `${date}:00` : date;

      await createWorkout({
        title,
        date: formattedDate,
        notes,
      });

      setTitle("");
      setDate("");
      setNotes("");
      setSuccessMessage("Träningspasset skapades.");
      
      await loadWorkouts();
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Kunde inte skapa träningspass."
      );
    }
  }

  async function handleDelete(id: number) {
    try {
      setError("");
      setSuccessMessage("");

      await deleteWorkout(id);
      setSuccessMessage("Träningspasset raderades.");

      await loadWorkouts();
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Kunde inte radera träningspasset."
      );
    }
  }

  return (
    <main className="min-h-screen bg-slate-50">
      <div className="mx-auto max-w-5xl px-6 py-10">
        <div className="mb-8">
          <h1 className="text-4xl font-bold tracking-tight text-slate-900">
            Dashboard
          </h1>
          <p className="mt-2 text-slate-600">
            Hantera dina träningspass och håll koll på din träning.
          </p>
        </div>

        {user && (
          <span
            className={`rounded-full px-4 py-2 text-sm font-medium ${
              user.role === "Admin"
                ? "bg-purple-100 text-purple-700"
                : "bg-slate-200 text-slate-700"
            }`}
          >
            Roll: {user.role}
          </span>
        )}

        {error && (
          <div className="mb-6 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-red-700">
            {error}
          </div>
        )}

        {successMessage && (
          <div className="mb-6 rounded-xl border border-green-200 bg-green-50 px-4 py-3 text-green-700">
            {successMessage}
          </div>
        )}

        <div className="grid gap-8 lg:grid-cols-[1fr_1.4fr]">
          <section className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-slate-200">
            <h2 className="mb-4 text-xl font-semibold text-slate-900">
              Skapa träningspass
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="mb-1 block text-sm font-medium text-slate-700">
                  Titel
                </label>
                <input
                  type="text"
                  placeholder="Till exempel Bröstpass"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  className="w-full rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-200"
                  required
                />
              </div>

              <div>
                <label className="mb-1 block text-sm font-medium text-slate-700">
                  Datum och tid
                </label>
                <input
                  type="datetime-local"
                  value={date}
                  onChange={(e) => setDate(e.target.value)}
                  className="w-full rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-200"
                  required
                />
              </div>

              <div>
                <label className="mb-1 block text-sm font-medium text-slate-700">
                  Anteckningar
                </label>
                <textarea
                  placeholder="Skriv något om passet..."
                  value={notes}
                  onChange={(e) => setNotes(e.target.value)}
                  className="min-h-[120px] w-full rounded-xl border border-slate-300 px-4 py-3 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-200"
                />
              </div>

              <button
                type="submit"
                className="w-full rounded-xl bg-blue-600 px-4 py-3 font-medium text-white transition hover:bg-blue-700"
              >
                Skapa träningspass
              </button>
            </form>
          </section>

          <section className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-slate-200">
            <div className="mb-4 flex items-center justify-between">
              <h2 className="text-xl font-semibold text-slate-900">
                Mina träningspass
              </h2>
              <span className="rounded-full bg-slate-100 px-3 py-1 text-sm text-slate-600">
                {workouts.length} st
              </span>
            </div>

            {loading ? (
              <p className="text-slate-600">Laddar träningspass...</p>
            ) : workouts.length === 0 ? (
              <div className="rounded-xl border border-dashed border-slate-300 p-6 text-center text-slate-500">
                Inga träningspass ännu.
              </div>
            ) : (
              <div className="space-y-4">
                {workouts.map((w) => (
                  <article
                    key={w.id}
                    className="rounded-2xl border border-slate-200 bg-slate-50 p-5 transition hover:shadow-sm"
                  >
                    <div className="flex items-start justify-between gap-4">
                      <div>
                        <h3 className="text-lg font-semibold text-slate-900">
                          {w.title}
                        </h3>
                        <p className="mt-1 text-sm text-slate-500">
                          {new Date(w.date).toLocaleString()}
                        </p>
                      </div>

                      <button
                        onClick={() => handleDelete(w.id)}
                        className="rounded-xl bg-red-500 px-4 py-2 text-sm font-medium text-white transition hover:bg-red-600"
                      >
                        Ta bort
                      </button>
                    </div>

                    {w.notes && (
                      <p className="mt-4 text-slate-700">{w.notes}</p>
                    )}
                  </article>
                ))}
              </div>
            )}
          </section>
        </div>
      </div>
    </main>
  );
}