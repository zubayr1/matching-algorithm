from typing import Optional

from fastapi import FastAPI

app = FastAPI()


@app.get("/")
def read_root():
    return [
            {"marketType": "fixedPrice",
            "isBlind": True,
            "flexRequestList":
            [
            {"RequestID": "Req000001",
            "timeSlot": "2021-10-05T15:00:00Z",
            "ifFlexRequested": True,
            "totalFlexRequestedEU": -11,
            "priceOfferedCtpEU": 7,
            "referencePriceCtpEU": 'null',
            "loc": {"user001": 3,"user002": 2,"user003":5,"user004":1},
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 50
            },
            {"RequestID": "Req000002",
            "timeSlot": "2021-10-05T15:15:00Z",
            "ifFlexRequested": True,
            "totalFlexRequestedEU": 8,
            "priceOfferedCtpEU": 9,
            "referencePriceCtpEU": 'null',
            "loc": {"user001": 4,"user002":7},
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 65

            },
            {"RequestID": "Req000003",
            "timeSlot": "2021-10-05T15:30:00Z",
            "ifFlexRequested": False,
            "totalFlexRequestedEU": 'null',
            "priceOfferedCtpEU ": 'null',
            "referencePriceCtpEU": 'null',
            "loc": 'null',
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 'null'

            },
            {"RequestID": "Req000004",
            "timeSlot": "2021-10-05T15:45:00Z",
            "ifFlexRequested": False,
            "totalFlexRequestedEU": 'null',
            "priceOfferedCtpEU ": 'null',
            "referencePriceCtpEU": 'null',
            "loc": 'null',
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 'null'
            },
            {"RequestID": "Req000005",
            "timeSlot": "2021-10-05T16:00:00Z",
            "ifFlexRequested": False,
            "totalFlexRequestedEU": 'null',
            "priceOfferedCtpEU ": 'null',
            "referencePriceCtpEU": 'null',
            "loc": 'null',
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 'null'
            }
            ]
            },

            {
            "marketType": "auction",
            "isBlind": True,
            "flexRequestList":
            [
            {"RequestID": "Req000006",
            "timeSlot": "2021-10-05T15:00:00Z",
            "ifFlexRequested": True,
            "totalFlexRequestedEU": 7,
            "priceOfferedCtpEU": 'null',
            "referencePriceCtpEU": 10,
            "loc": {"user001": 3,"user002": 2,"user003":5,"user004":1},
            "maxPriceCtpEU": 12,
            "fullfillmenttFactor": 80
            },
            {"RequestID": "Req000007",
            "timeSlot": "2021-10-05T15:15:00Z",
            "ifFlexRequested": True,
            "totalFlexRequestedEU": 5,
            "priceOfferedCtpEU": 'null',
            "referencePriceCtpEU": 7,
            "loc": {"user001": 4,"user002":7, "user003":1},
            "maxPriceCtpEU": 10,
            "fullfillmenttFactor": 70
            },
            {"RequestID": "Req000008",
            "timeSlot": "2021-10-05T15:30:00Z",
            "ifFlexRequested": False,
            "totalFlexRequestedEU": 'null',
            "priceOfferedCtpEU ": 'null',
            "referencePriceCtpEU": 'null',
            "loc": 'null',
            "maxPriceCtpEU": 'null',
            "fullfillmenttFactor": 'null'
            }
            ]
            }
            
        ]




@app.get("/users/user001")
def read_item():
    return {
        "userID": "user001",
        "flexOfferList":
        [
        {"RequestID": "Req000001",
        "totalFlexOfferedEU": -4,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T21:00:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000002",
        "totalFlexOfferedEU": -2,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000006",
        "totalFlexOfferedEU": 4,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [8, 10, 12, 13]
        },
        {"RequestID": "Req000007",
        "totalFlexOfferedEU": 4,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [7]
        }
        ]
        }




@app.get("/users/user002")
def read_item():
    return {
        "userID": "user002",
        "flexOfferList":
        [
        {"RequestID": "Req000001",
        "totalFlexOfferedEU": -6,
        "startFlexShiftTimeslot ": "2021-10-05T16:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T20:00:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000002",
        "totalFlexOfferedEU": 3,
        "startFlexShiftTimeslot ": "2021-10-05T19:10:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:18:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000006",
        "totalFlexOfferedEU": 3,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [6, 10, 14]
        },
        {"RequestID": "Req000007",
        "totalFlexOfferedEU": 5,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [8, 8]
        }
        ]
        }



@app.get("/users/user003")
def read_item():
    return {
        "userID": "user003",
        "flexOfferList":
        [
        {"RequestID": "Req000001",
        "totalFlexOfferedEU": -2,
        "startFlexShiftTimeslot ": "2021-10-05T20:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T23:00:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000002",
        "totalFlexOfferedEU": 4,
        "startFlexShiftTimeslot ": "2021-10-05T19:04:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:13:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000006",
        "totalFlexOfferedEU": 3,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [11, 13, 15]
        },
        {"RequestID": "Req000007",
        "totalFlexOfferedEU": 5,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [15]
        }
        ]
        }



@app.get("/users/user004")
def read_item():
    return {
        "userID": "user004",
        "flexOfferList":
        [
        {"RequestID": "Req000001",
        "totalFlexOfferedEU": -5,
        "startFlexShiftTimeslot ": "2021-10-05T18:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000002",
        "totalFlexOfferedEU": 3,
        "startFlexShiftTimeslot ": "2021-10-05T20:04:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T20:13:00Z",
        "bidPriceCtpEUList": 'null'
        },
        {"RequestID": "Req000006",
        "totalFlexOfferedEU": 5,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [5, 7, 9, 11, 14]
        },
        {"RequestID": "Req000007",
        "totalFlexOfferedEU": 3,
        "startFlexShiftTimeslot ": "2021-10-05T19:00:00Z",
        "endFlexShiftTimeslot ": "2021-10-05T19:15:00Z",
        "bidPriceCtpEUList": [9, 13, 15]
        }
        ]
        }



