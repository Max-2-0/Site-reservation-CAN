// reservation.js

document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formReservation");
    const zonePassagers = document.getElementById("zonePassagers");
    const zoneVehicules = document.getElementById("zoneVehicules");
    const btnAjoutPassager = document.getElementById("btnAjoutPassager");
    const btnAjoutVehicule = document.getElementById("btnAjoutVehicule");
    const apercu = document.getElementById("aperçuJson");
    const message = document.getElementById("message");

    btnAjoutPassager.addEventListener("click", () => {
        const ligne = document.createElement("div");
        ligne.className = "ligne-passager";
        ligne.innerHTML = `
            <input type="text" class="nomP" placeholder="Nom" required>
            <input type="text" class="prenomP" placeholder="Prénom" required>
            <select class="codeCatP" required>
                <option value="adu26p">Adulte 26 ans et plus</option>
                <option value="jeu1825">Jeune 18 à 25 ans inclus</option>
                <option value="enf417">Enfant 4 à 17 ans inclus</option>
                <option value="bebe">Bébé moins de 4 ans</option>
                <option value="ancomp">Animal de compagnie</option>
            </select>
        `;
        zonePassagers.appendChild(ligne);
    });

    btnAjoutVehicule.addEventListener("click", () => {
        const ligne = document.createElement("div");
        ligne.className = "ligne-vehicule";
        ligne.innerHTML = `
            <select class="codeCatV">
                <option value="">(aucun)</option>
                <option value="trot">Trottinette électrique</option>
                <option value="velo">Vélo ou remorque à vélo</option>
                <option value="velelec">Vélo électrique</option>
                <option value="cartand">Vélo cargo / tandem</option>
                <option value="mobil">Deux-roues ≤ 125 cm³</option>
                <option value="moto">Deux-roues &gt; 125 cm³</option>
                <option value="cat1">Voiture &lt; 4 m</option>
                <option value="cat2">Voiture 4 – 4.39 m</option>
                <option value="cat3">Voiture 4.40 – 4.79 m</option>
                <option value="cat4">Voiture 4.80 m et plus</option>
                <option value="camp">Camping-car &gt; 2.10 m</option>
            </select>
            <input type="number" class="quantiteV" min="1" value="1">
        `;
        zoneVehicules.appendChild(ligne);
    });

    form.addEventListener("submit", (e) => {
        e.preventDefault();

        const nomReservation = document.getElementById("nomReservation").value.trim();
        const idLiaison = parseInt(document.getElementById("liaison").value, 10);
        const date = document.getElementById("date").value; // yyyy-mm-dd
        const heure = document.getElementById("heure").value;
        const horodatage = new Date().toISOString().slice(0, 19).replace("T", " ");

        const passagers = [];
        zonePassagers.querySelectorAll(".ligne-passager").forEach(ligne => {
            const nom = ligne.querySelector(".nomP").value.trim();
            const prenom = ligne.querySelector(".prenomP").value.trim();
            const codeCategorie = ligne.querySelector(".codeCatP").value;
            if (nom && prenom) {
                passagers.push({ nom, prenom, codeCategorie });
            }
        });

        const vehicules = [];
        zoneVehicules.querySelectorAll(".ligne-vehicule").forEach(ligne => {
            const codeCategorie = ligne.querySelector(".codeCatV").value;
            const qte = parseInt(ligne.querySelector(".quantiteV").value, 10) || 0;
            if (codeCategorie && qte > 0) {
                vehicules.push({ codeCategorie, quantite: qte });
            }
        });

        const reservation = {
            nom: nomReservation,
            idLiaison: idLiaison,
            date: date,
            heure: heure,
            horodatage: horodatage,
            passagers: passagers,
            vehicules: vehicules
        };

        const payload = [reservation]; // tableau, comme dans ton C#
        apercu.textContent = JSON.stringify(payload, null, 2);
        message.textContent = "Réservation prête à être envoyée au programme C#.";

        // Quand tu auras un endpoint ou une méthode pour récupérer ce JSON,
        // tu pourras faire :
        // fetch('http://localhost:5000/api/reservation', {
        //     method: 'POST',
        //     headers: { 'Content-Type': 'application/json' },
        //     body: JSON.stringify(payload)
        // });
    });
});
