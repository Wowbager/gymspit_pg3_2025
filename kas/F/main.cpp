#include <bits/stdc++.h>
#include <string>
#include <algorithm>
#include <iostream>
#include <vector>
#include <queue>
#include <chrono>
#include <ctime>
#include <thread>

#define l long
#define u unsigned

using namespace std;

struct head
{
    int traceback;
    int depth;
    int active_node;
};

struct node
{
    bool visited;
    vector<int> neighbours;
    head* active_head = nullptr;
    vector<int> paths;
    int p_loop = -1;
};

struct path
{
    int node1;
    int node2;
    bool direction_definitive = false;
    bool dir_from_1_to_2;
};

struct loop
{
    int p_loop;
    int size;
};

struct trace
{
    int path_i;
    int trace_i;
};



// stolen from https://www.geeksforgeeks.org/cpp/how-to-make-a-countdown-timer-in-cpp/
string getPresentDateTime()
{
    // Declare a time_t variable to hold the current time.
    time_t tt;
    // Declare a pointer to a tm struct to hold the local
    // time.
    struct tm* st;

    // Get the current time.
    time(&tt);
    // Convert the current time to local time.
    st = localtime(&tt);
    // Return the local time as a string.
    return asctime(st);
}

template <typename I, typename O>
void precti_vstup(I &vstup, O &vystup) {
    vstup.exceptions(ios::badbit | ios::failbit);
    vystup.exceptions(ios::badbit | ios::failbit);

    int pocet_problemu;
    vstup >> pocet_problemu;

    for (int i = 0; i < pocet_problemu; i++) {
        int N;
        int M;

        vstup >> N >> M;

        if (N > M){
            vystup << "0\n";
            for (int i = 0; i < M; i++){
                int tmp;
                vstup >> tmp;
                vystup << tmp;
                vstup >> tmp;
                vystup << tmp << "\n";
            }
        }

        node *nodes;
        nodes = new node[N];

        path *paths;
        paths = new path[M];

        vector<trace> traceback;

        for (int i = 0; i < M; i++){
            int tmp1;
            int tmp2;

            vstup >> tmp1 >> tmp2;

            nodes[tmp1].neighbours.push_back(tmp2);
            nodes[tmp2].neighbours.push_back(tmp1);

            nodes[tmp1].paths.push_back(i);
            nodes[tmp2].paths.push_back(i);

            paths[i].node1 = tmp1;
            paths[i].node1 = tmp2;
        }

        int starting_node;

        for (int i = 0; i < M; i++){
            if (nodes[i].neighbours.size() > 1){
                starting_node = i;
                break;
            }
        }

        queue<head> heads;

        head tmp_head;
        tmp_head.active_node = starting_node;
        tmp_head.depth = 0;
        tmp_head.traceback = 0;
        heads.push(tmp_head);

        nodes[starting_node].active_head = &tmp_head;
        nodes[starting_node].visited = true;

        traceback.push_back();
        traceback.at(0).path_i = -1;
        traceback.at(0).trace_i = -1

        while (!heads.empty()){
            int depth;
            int trace;
            head tmp_head;
            tmp_head = heads.front();
            heads.pop();

            depth = tmp_head.depth;
            trace = tmp_head.traceback;
            if (nodes[i].neighbours.size() == 1){
                cout << "found dead end (not handled yet)\n";
            }

            for (int node_i:  nodes[tmp_head.active_node].neighbours){
                if (nodes[node_i].active_head == nullptr){
                    cout << "handle cycle detection\n";
                }
                else if (!nodes[node_i].visited){
                    tmp_head.depth = depth + 1;
                    tmp_head.active_node = node_i;
                    tmp_head.traceback = traceback.size();
                    traceback.pushback();
                    traceback.at(tmp_head.traceback).path_i
                }
            }
        }

        delete nodes;
        delete paths;
    }
} 

const array<pair<string, string>, 2> nazvy_souboru = {
    make_pair("F-lehky.txt", "F-lehky-vystup.txt"),
    make_pair("F-tezky.txt", "F-tezky-vystup.txt"),
};

int main() {
    bool soubor_nalezen = false;

    for (auto nazvy : nazvy_souboru) {
        ifstream vstup(nazvy.first);
        if (vstup.fail())
            continue;

        ofstream vystup(nazvy.second);

        precti_vstup(vstup, vystup);

        soubor_nalezen = true;
    }

    if (!soubor_nalezen)
        precti_vstup(cin, cout);
} 