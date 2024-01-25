from datetime import datetime
import json
import requests
import aiohttp

'''
	API Trenitalia
	Version: 0.1.0
	Visual Laser 10 New - 10/2023
'''

class apitr:
	__decodeJson = True
	def __init__(self, decodeJson:bool = True):
		# decodeJson: True = return dict, False = return text/plain
		self.__decodeJson = decodeJson

	__uris = {
		'InfoMob': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/infomobilitaTicker/',
		'partenze': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/partenze/',
		'arrivi': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/arrivi/',
		'andamento': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/andamentoTreno/',
		'indicazioniViaggio': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/soluzioniViaggioNew/',
		'searchStazione': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/cercaStazione/',
		'StazioniByRegione': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/elencoStazioni/',
     	'autocompletaStazione': 'http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/autocompletaStazione/'
	}

	__datetimeFormat = {
		'partenze':'%a %b %d %Y %H:%M:%S GMT+0200 (Ora legale dell’Europa centrale)',
		'arrivi':'%a %b %d %Y %H:%M:%S GMT+0200 (Ora legale dell’Europa centrale)',
		'andamento':'timestamp',
		'indicazioniViaggio':'%Y-%m-%dT%H:%M:%S'	#YYYY-MM-DDTHH:MM:SS
	}
 
	def printBeautify(self, json_object):
		print(json.dumps(json_object, indent=2))

	def __dateTime2Str(self,date: datetime, format:str):
		if (format == 'timestamp'):
			return str(int(date.timestamp()))+'000'
		else:
			return date.strftime(format)
	
	async def __request(self, uri):
		async with aiohttp.ClientSession() as session:
			async with session.get(uri) as resp:
				if (resp.status == 200):
					try:
						if (self.__decodeJson):
							return await resp.json()
						else:
							return await resp.text()
					except:
						return resp.text()
				else:
					print(resp.status)
					return None

	def __minimizeCodStazione(self, codStazione:str):
		codStazione = codStazione[1:]
		return str(int(codStazione))

	def __adjustNomeStazione(self, nomeStazione:str) -> str:
		return nomeStazione[0:20]

	####### PUBLIC METHODS #######
	async def getInfoMob(self):
		# GET /resteasy/viaggiatreno/infomobilitaTicker/
		return await self.__request(self.__uris['InfoMob'])

	async def getPartenze(self, idStazione:str, dataora: datetime):
		# GET /resteasy/viaggiatreno/partenze/{codiceStazione}/{orario}
		return await self.__request(self.__uris['partenze'] + idStazione 
								+ '/' + self.__dateTime2Str(dataora, self.__datetimeFormat['partenze']))

	async def getArrivi(self, idStazione:str, dataora: datetime):
		# GET /resteasy/viaggiatreno/arrivi/{codiceStazione}/{orario}
		return await self.__request(self.__uris['arrivi'] + idStazione 
								+ '/' + self.__dateTime2Str(dataora, self.__datetimeFormat['arrivi']))

	async def getAndamento(self, idStazioneOrigine:str, idTreno:str, dataoraPartenza: datetime):
		# GET /resteasy/viaggiatreno/andamentoTreno/{codOrigine}/{numeroTreno}/{dataPartenza}
		return await self.__request(self.__uris['andamento'] + idStazioneOrigine + '/' + idTreno 
								+ '/' + self.__dateTime2Str(dataoraPartenza, self.__datetimeFormat['andamento']))

	async def getIndicazioniViaggio(self, idStazioneOrigine: str, idStazioneArrivo:str, dataora: datetime):
		# GET /resteasy/viaggiatreno/soluzioniViaggioNew/{codLocOrig}/{codLocDest}/{date}
		idStazioneArrivo = self.__minimizeCodStazione(idStazioneArrivo)
		idStazioneOrigine = self.__minimizeCodStazione(idStazioneOrigine)

		p = self.__uris['indicazioniViaggio'] + idStazioneOrigine + '/' + idStazioneArrivo + '/' + self.__dateTime2Str(dataora, self.__datetimeFormat['indicazioniViaggio'])
		return await self.__request(p)

	async def searchStazione(self, nomeStazione:str):
		# GET /resteasy/viaggiatreno/cercaStazione/{text}
		return await self.__request(self.__uris['searchStazione'] + self.__adjustNomeStazione(nomeStazione))

	async def getStazioniByRegione(self, codRegione:str):
		# GET /resteasy/viaggiatreno/elencoStazioni/{regione}
		return await self.__request(self.__uris['StazioniByRegione'] + codRegione)

	async def getAutoCompletaStazione(self, nomeStazione:str):
		# GET /resteasy/viaggiatreno/autocompletaStazione/{stazione}
		return await self.__request(self.__uris['autocompletaStazione'] + self.__adjustNomeStazione(nomeStazione))

	async def getCodStazione(self, nomeStazione:str):
		# return codStazione from nomeStazione
		stazioni = await self.searchStazione(nomeStazione)
		if (stazioni != None):
			try:
				for stazione in stazioni: 
					if stazione['nomeLungo'].lower() == nomeStazione.lower():
						return stazione['id']
				#return [stazione['id'] for stazione in stazioni if stazione['nomeLungo'].lower() == nomeStazione.lower()][0]
			except:
				return None
		else:
			return None
        	
	####### TOOLS METHODS #######
	def timestamp2datetime(self, timestamp):
		# convert timestamp with || without millisec to datetime
		try:
			return datetime.fromtimestamp(int(timestamp))
		except:
			return datetime.fromtimestamp(int(timestamp)/1000)
