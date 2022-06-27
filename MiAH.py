import sys
from work import  returnfulldata, returnuserdata
from collections import OrderedDict

alldata = returnfulldata()


def matching(userlist, value, TOTALFLEXREQUESTED):
    accepted_offers = {}
    POTENTIALOFFER={}

    SIGN= 1
    if TOTALFLEXREQUESTED<0:
        SIGN=-1
        TOTALFLEXREQUESTED = abs(TOTALFLEXREQUESTED)

    for user in userlist:
        userdata, username = returnuserdata(user)
        
        for uservalue in userdata['flexOfferList']:
            if value['RequestID']== uservalue['RequestID']:
                if (SIGN>0 and uservalue['totalFlexOfferedEU']> 0) or (SIGN<0 and uservalue['totalFlexOfferedEU']<0):
                    
                    if SIGN<0 and uservalue['totalFlexOfferedEU']<0:
                        uservalue['totalFlexOfferedEU'] = abs(uservalue['totalFlexOfferedEU'])
                    
                    POTENTIALOFFER[username] = uservalue['totalFlexOfferedEU']

                
    SUM=0
    for i in POTENTIALOFFER:
        SUM+= POTENTIALOFFER[i]
    
    if SUM<=TOTALFLEXREQUESTED:
        return POTENTIALOFFER
    else:
        d_sorted_by_value = OrderedDict(sorted(POTENTIALOFFER.items(), key=lambda x: x[1], reverse=True))

        for i in d_sorted_by_value:
            if TOTALFLEXREQUESTED> d_sorted_by_value[i]:
                accepted_offers[i] = d_sorted_by_value[i]
                TOTALFLEXREQUESTED-=d_sorted_by_value[i]
            else:
                accepted_offers[i] = TOTALFLEXREQUESTED
                return accepted_offers
        
        return accepted_offers


def checkFulmentFactor(accepted_offers, value,TOTALFLEXREQUESTED):
    if sum(accepted_offers.values())/abs(TOTALFLEXREQUESTED) * 100 >= value:
        return True
    return False

def miah():
    accepted_offers = {}
    final_accepted_offers = {}

    print('Minimale Anzahl HEMS (MiAH)')

    for i in alldata:
        if i['marketType']=='fixedPrice':
            for value in i['flexRequestList']:
                if (value['ifFlexRequested'])==False:
                    continue
                print('\n\n\n')
                print('Request for '+value['RequestID']+" "+ str(value['totalFlexRequestedEU']))
                TOTALFLEXREQUESTED = value['totalFlexRequestedEU']
                print('\n')
                userlist = value['loc'].keys()

                accepted_offers = matching(userlist, value, TOTALFLEXREQUESTED)

                if checkFulmentFactor(accepted_offers, value['fullfillmenttFactor'],TOTALFLEXREQUESTED):                
                    final_accepted_offers[value['RequestID']] = accepted_offers
                else:
                    final_accepted_offers[value['RequestID']] = 'fullment factor did not match'

    print(final_accepted_offers)
    return final_accepted_offers




if __name__ == '__main__':
    miah()



#   py .\MiAH.py 