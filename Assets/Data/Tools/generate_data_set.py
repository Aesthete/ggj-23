import random

def run():
    with open('test_node_set.csv', 'w') as f:
        node_count = 20
        xy = []
        for x in range (node_count):
            xy = [random.uniform(0, 1.0), random.uniform(0, 1.0)]
            xy += [random.randrange(0, 20) for x in range(0,random.randrange(1, 4))]
            f.write(",".join(str(x) for x in xy) + "\n")

if __name__ == "__main__":
    run()