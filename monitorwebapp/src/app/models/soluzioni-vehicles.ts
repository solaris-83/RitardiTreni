export interface Vehicle {
  origine: string;
  destinazione: string;
  orarioPartenza: Date;
  orarioArrivo: Date;
  categoria: string;
  categoriaDescrizione: string;
  numeroTreno: string;
}

export interface Soluzioni {
  durata: string;
  vehicles: Vehicle[];
}

export interface SoluzioniVehicle {
  soluzioni: Soluzioni[];
  origine: string;
  destinazione: string;
  errore?: any;
}
