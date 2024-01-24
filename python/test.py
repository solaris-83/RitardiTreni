import unittest
from apitr import apitr
from datetime import datetime

class TestApitr(unittest.IsolatedAsyncioTestCase):
    async def test_getInfoMob(self):
        self.assertIsNotNone(apitr(False).getInfoMob())
        
    async def test_searchStazione(self):
        a = await apitr().searchStazione('BIELLA S.PAOLO')
        self.assertEqual(a[0]['nomeLungo'], 'BIELLA S.PAOLO', 'Station name is different')  
        a = await apitr().searchStazione('SANTHIA')
        self.assertEqual(a[0]['nomeLungo'], 'SANTHIA`', 'Station name is different')
        a = await apitr().searchStazione('NOVARA')
        self.assertEqual(a[0]['nomeLungo'], 'NOVARA', 'Station name is different')  
        
    async def test_getCodStazione(self):
        a = await apitr().getCodStazione('BIELLA S.PAOLO')   
        self.assertEqual(a, 'S00070', 'Codes do not match') 
        a = await apitr().getCodStazione('SANTHIA`')   
        self.assertEqual(a, 'S00240', 'Codes do not match') 
        a = await apitr().getCodStazione('NOVARA')   
        self.assertEqual(a, 'S00248', 'Codes do not match')  
        
	
    async def test_getPartenze(self):
        a = await apitr().getPartenze('S00070', datetime.today())
        self.assertTrue(len(a) > 0, 'getPartenze empty list')
        a = await apitr().getPartenze('S00240', datetime.today())
        self.assertTrue(len(a) > 0, 'getPartenze empty list')
        a = await apitr().getPartenze('S00248', datetime.today())
        self.assertTrue(len(a) > 0, 'getPartenze empty list')
        
if __name__ == "__main__":
    unittest.main()