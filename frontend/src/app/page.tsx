import Link from "next/link";

export default function HomePage() {
  return (
    <main className="min-h-screen bg-gradient-to-br from-gray-900 via-gray-800 to-black text-white flex items-center justify-center px-6">
      <div className="max-w-4xl w-full text-center">

        {/* Logo / Titel */}
        <h1 className="text-5xl font-extrabold mb-6 tracking-tight">
          GymApp
        </h1>

        {/* Tagline */}
        <p className="text-xl text-gray-300 mb-10">
          Din personliga träningspartner 💪 <br />
          Planera pass, följ din utveckling och nå dina mål.
        </p>

        {/* CTA knappar */}
        <div className="flex flex-col sm:flex-row gap-4 justify-center mb-16">
          <Link
            href="/login"
            className="bg-blue-600 hover:bg-blue-700 transition px-8 py-4 rounded-xl font-semibold text-lg shadow-lg"
          >
            Logga in
          </Link>

          <Link
            href="/register"
            className="bg-white text-black hover:bg-gray-200 transition px-8 py-4 rounded-xl font-semibold text-lg shadow-lg"
          >
            Skapa konto
          </Link>
        </div>

        {/* Features */}
        <div className="grid md:grid-cols-3 gap-6 text-left">
          <div className="bg-gray-800 p-6 rounded-xl shadow-md">
            <h3 className="font-bold text-lg mb-2">📋 Planera</h3>
            <p className="text-gray-400">
              Skapa och strukturera dina träningspass enkelt.
            </p>
          </div>

          <div className="bg-gray-800 p-6 rounded-xl shadow-md">
            <h3 className="font-bold text-lg mb-2">📈 Följ utveckling</h3>
            <p className="text-gray-400">
              Se dina framsteg över tid och håll motivationen uppe.
            </p>
          </div>

          <div className="bg-gray-800 p-6 rounded-xl shadow-md">
            <h3 className="font-bold text-lg mb-2">🔐 Säker inloggning</h3>
            <p className="text-gray-400">
              Din data är skyddad med JWT-autentisering.
            </p>
          </div>
        </div>

      </div>
    </main>
  );
}