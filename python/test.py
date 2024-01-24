from time import strftime
import unittest
from apitr import apitr
import datetime

class TestApitr(unittest.IsolatedAsyncioTestCase):
    
    dict_station = {}

    def __init__(self, methodName: str = "runTest") -> None:
        super().__init__(methodName)
        self.dict_station = { "BIELLA S.PAOLO": "S00070", "NOVARA": "S00248", "SANTHIA`": "S00240"}
    
    async def test_getInfoMob(self):
        self.assertIsNotNone(await apitr(False).getInfoMob())
        
    async def test_searchStazione(self):
        for key in self.dict_station.keys():
            a = await apitr().searchStazione(key)
            self.assertEqual(a[0]['nomeLungo'], key, 'Station name is different')  
        
    async def test_getCodStazione(self):
        for key in self.dict_station.keys():
            a = await apitr().getCodStazione(key)   
            self.assertEqual(a, self.dict_station[key], 'Codes do not match')      
	
    async def test_getPartenze(self):
        for key in self.dict_station.keys():
            a = await apitr().getPartenze(self.dict_station[key], datetime.date.today())
            self.assertIsNotNone(a, 'getPartenze empty list')
        
    async def test_getArrivi(self):
        for key in self.dict_station.keys():
            a = await apitr().getArrivi(self.dict_station[key], datetime.date.today())
            self.assertIsNotNone(a, 'getArrivi empty list')
    
    async def test_getAndamento(self):
        for key in self.dict_station.keys():  
            a = await apitr().getPartenze(self.dict_station[key], datetime.date.today())
            for i in a:
                b = await apitr().getAndamento(self.dict_station[key], str(i['numeroTreno']), datetime.strptime(i['orarioPartenza'], '%Y-%m-%dT%H:%M:%S'))
                self.assertIsNotNone(b['numeroTreno'], 'Could not retrieve train code')
                self.assertTrue(len(b['fermate']) > 0, 'Fermate empty list')
            
if __name__ == "__main__":
    unittest.main()