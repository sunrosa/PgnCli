import sys
import chess.pgn
import json

def import_all_headers(pgn):
    headers = []

    while True:
        header = chess.pgn.read_headers(pgn)

        if header == None:
            break

        headers.append(header)

    return headers

def main():
    pgn = open(sys.argv[1])
    print(json.dumps([obj.__dict__ for obj in import_all_headers(pgn)]))

if __name__ == "__main__":
    main()
