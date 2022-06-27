import requests


RequestURL = 'https://flexchain-functionapp-testenv.azurewebsites.net/api/flexibilityRequests/'

OfferURL = 'https://flexchain-functionapp-testenv.azurewebsites.net/api/flexibilityOffers/'

r = requests.get(url = RequestURL)

data = r.json()

print(data)

def returnfulldata():
    return data



def returnuserdata(user):
    r = requests.get(url = OfferURL+'users/'+user)


    data = r.json()
    return data, user



