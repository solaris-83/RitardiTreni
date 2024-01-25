from time import strftime
import unittest
from apitr import apitr
import datetime

class TestApitr(unittest.IsolatedAsyncioTestCase):
    
    dict_stationAndCodes = {}

    def __init__(self, methodName: str = "runTest") -> None:
        super().__init__(methodName)
        self.dict_stationAndCodes = {"MILANO PORTA GARIBALDI": "S01645", "BIELLA S.PAOLO": "S00070", "NOVARA": "S00248", "SANTHIA`": "S00240", 
                                     "CARPIGNANO SESIA" : "S00971", "CHIVASSO": "S00232", "COSSATO": "S00973", 
                                     "MAGENTA": "S01040", "MILANO CENTRALE": "S01700", "RHO FIERA MILANO": "S01039", 
                                     "ROVASENDA": "S00053", "SALUSSOLA": "S00074", "SANTHIA`": "S00240", 
                                     "TORINO P.NUOVA": "S00219", "TORINO PORTA SUSA": "S00035", "VERCELLI": "S00245"}
    
    async def test_getInfoMob(self):
        self.assertIsNotNone(await apitr(False).getInfoMob())
        
    async def test_searchStazione(self):
        for key in self.dict_stationAndCodes.keys():
            a = await apitr().searchStazione(key)
            self.assertEqual(a[0]['nomeLungo'], key, 'Station name is different')  
        
    async def test_getCodStazione(self):
        for key in self.dict_stationAndCodes.keys():
            a = await apitr().getCodStazione(key)   
            self.assertEqual(a, self.dict_stationAndCodes[key], 'Codes do not match')      
	
    async def test_getPartenze(self):
        for key in self.dict_stationAndCodes.keys():
            a = await apitr().getPartenze(self.dict_stationAndCodes[key], datetime.date.today())
            self.assertIsNotNone(a, 'getPartenze empty list')
        
    async def test_getArrivi(self):
        for key in self.dict_stationAndCodes.keys():
            a = await apitr().getArrivi(self.dict_stationAndCodes[key], datetime.date.today())
            self.assertIsNotNone(a, 'getArrivi empty list')
    
    async def test_getAndamento(self):
        for key in self.dict_stationAndCodes.keys():  
            a = await apitr().getPartenze(self.dict_stationAndCodes[key], datetime.date.today())
            for i in a:
                b = await apitr().getAndamento(self.dict_stationAndCodes[key], str(i['numeroTreno']), datetime.strptime(i['orarioPartenza'], '%Y-%m-%dT%H:%M:%S'))
                self.assertIsNotNone(b['numeroTreno'], 'Could not retrieve train code')
                self.assertTrue(len(b['fermate']) > 0, 'Fermate empty list')
                
    async def test_getAutoCompletaStazione(self):
        for key in self.dict_stationAndCodes.keys():
            a = await apitr(False).getAutoCompletaStazione(key)
            self.assertIsNotNone(a, 'test_getAutoCompletaStazione empty result')
                        
if __name__ == "__main__":
    unittest.main()