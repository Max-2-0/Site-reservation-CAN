function afficherCarteEmbarquement() {
    let GareDepart = document.getElementById("comboGareDepart");
    let Gare_dArrivee = document.getElementById("comboGaredArrivee");
    let Date = document.getElementById("comboDate");
    let HDep = document.getElementById("comboHDep");
    let Bateau = document.getElementById("comboBateau");

    const getEmbarquement = async () => {
        try {
            const response = await fetch(); // API de Clément 
            
            if (!response.ok) {
                alert("Erreur de réseau");
				return;
            }
            const data = await response.json();
            console.log(data);
            comboGareDepart.innerHTML = "";
            comboGaredArrivee.innerHTML = "";
            comboDate.innerHTML = "";
            comboHDep.innerHTML = "";
            comboBateau.innerHTML = "";
            data.forEach(item => {
                comboGareDepart.innerHTML += // API de Clément
                comboGaredArrivee.innerHTML += // API de Clément
                comboDate.innerHTML += // API de Clément
                comboHDep.innerHTML += // API de Clément
                comboBateau.innerHTML += // API de Clément
            });
        } catch (error) {
            console.error("Une erreur est survenue :", error);
        }
    };
    getEmbarquement();
}
afficherCarteEmbarquement();