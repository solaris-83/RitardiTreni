import unittest
from Python.apitr import apitr

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
        
        
if __name__ == "__main__":
    unittest.main()