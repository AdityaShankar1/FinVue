<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import { Landmark, PlusCircle, RefreshCw, Trash2, Wallet, ArrowUpRight, Trophy, BarChart3, Fingerprint } from 'lucide-vue-next';
import { Pie } from 'vue-chartjs';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';

ChartJS.register(ArcElement, Tooltip, Legend);

interface Fund {
  id: number;
  name: string;
  ticker: string;
  nav: number;
}

const funds = ref<Fund[]>([]);
const loading = ref(false);
const newFund = ref({ name: '', ticker: '', nav: 0 });
const API_URL = 'http://localhost:5226/api/funds';

const fetchFunds = async () => {
  loading.value = true;
  try {
    const res = await axios.get(API_URL);
    funds.value = res.data;
  } finally { loading.value = false; }
};

// --- Computed Statistics ---
const totalValue = computed(() => funds.value.reduce((sum, f) => sum + f.nav, 0));

const topPerformer = computed(() => {
  if (funds.value.length === 0) return 'N/A';
  return funds.value.reduce((prev, current) => (prev.nav > current.nav) ? prev : current).ticker;
});

const avgNav = computed(() => {
  if (funds.value.length === 0) return 0;
  return totalValue.value / funds.value.length;
});

const addFund = async () => {
  if (!newFund.value.name) return;
  await axios.post(API_URL, newFund.value);
  newFund.value = { name: '', ticker: '', nav: 0 };
  fetchFunds();
};

const deleteFund = async (id: number) => {
  if (confirm("Delete asset?")) {
    await axios.delete(`${API_URL}/${id}`);
    fetchFunds();
  }
};

const chartData = computed(() => ({
  labels: funds.value.map(f => f.ticker),
  datasets: [{
    backgroundColor: ['#10b981', '#3b82f6', '#f59e0b', '#ef4444', '#8b5cf6'],
    data: funds.value.map(f => f.nav),
    borderWidth: 0
  }]
}));

onMounted(fetchFunds);
</script>

<template>
  <div class="min-h-screen bg-[#020617] p-8" style="color: #f8fafc; font-family: sans-serif;">

    <header class="max-w-6xl mx-auto mb-10 flex justify-between items-end border-b border-slate-800 pb-8">
      <div>
        <h1 class="text-4xl font-black italic" style="color: #ffffff;">Fin<span style="color: #10b981; font-style: normal;">Vue</span></h1>
        <p style="color: #10b981; font-size: 12px; font-weight: 800; letter-spacing: 1px;">INTELLIGENT FINANCIAL VIEW</p>
      </div>

      <div class="bg-slate-900 p-6 rounded-2xl border border-emerald-500/20 shadow-2xl shadow-emerald-900/10">
        <p style="color: #64748b; font-size: 10px; font-weight: 900; text-transform: uppercase; margin-bottom: 4px;">Total Portfolio Value</p>
        <p style="color: #10b981; font-size: 28px; font-weight: 900; font-family: monospace;">₹{{ totalValue.toLocaleString() }}</p>
      </div>
    </header>

    <div class="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
      <div class="bg-slate-900/50 border border-slate-800 p-4 rounded-2xl flex items-center gap-4">
        <Trophy class="text-amber-500" />
        <div>
          <p class="text-[10px] text-slate-500 font-black uppercase">Top Performer</p>
          <p class="font-bold text-slate-200">{{ topPerformer }}</p>
        </div>
      </div>
      <div class="bg-slate-900/50 border border-slate-800 p-4 rounded-2xl flex items-center gap-4">
        <Fingerprint class="text-blue-500" />
        <div>
          <p class="text-[10px] text-slate-500 font-black uppercase">Asset Count</p>
          <p class="font-bold text-slate-200">{{ funds.length }} Unique Holdings</p>
        </div>
      </div>
      <div class="bg-slate-900/50 border border-slate-800 p-4 rounded-2xl flex items-center gap-4">
        <BarChart3 class="text-emerald-500" />
        <div>
          <p class="text-[10px] text-slate-500 font-black uppercase">Avg. NAV</p>
          <p class="font-bold text-emerald-400 font-mono">₹{{ avgNav.toFixed(2) }}</p>
        </div>
      </div>
    </div>

    <main class="max-w-6xl mx-auto grid gap-8 lg:grid-cols-12">
      <section class="lg:col-span-4 bg-slate-900 p-8 rounded-3xl border border-slate-800 shadow-xl">
        <h2 class="text-xl font-bold mb-6 flex items-center gap-2" style="color: #ffffff;"><PlusCircle /> Execute Trade</h2>
        <div class="space-y-6">
          <div>
            <label style="color: #64748b; font-size: 11px; font-weight: 800; display: block; margin-bottom: 8px;">ASSET NAME</label>
            <input v-model="newFund.name" placeholder="e.g. Quant Flexi" class="w-full bg-[#020617] border border-slate-700 rounded-xl p-3 text-white outline-none focus:border-emerald-500" />
          </div>
          <div>
            <label style="color: #64748b; font-size: 11px; font-weight: 800; display: block; margin-bottom: 8px;">TICKER</label>
            <input v-model="newFund.ticker" placeholder="TICKER" class="w-full bg-[#020617] border border-slate-700 rounded-xl p-3 text-white outline-none focus:border-emerald-500 uppercase" />
          </div>
          <div>
            <label style="color: #64748b; font-size: 11px; font-weight: 800; display: block; margin-bottom: 8px;">NAV AMOUNT</label>
            <input v-model.number="newFund.nav" type="number" class="w-full bg-[#020617] border border-slate-700 rounded-xl p-3 text-emerald-400 font-bold outline-none focus:border-emerald-500" />
          </div>
          <button @click="addFund" class="w-full bg-emerald-600 hover:bg-emerald-500 text-white font-black py-4 rounded-xl transition-all shadow-lg shadow-emerald-900/40">
            CONFIRM ASSET
          </button>
        </div>
      </section>

      <section class="lg:col-span-8 space-y-8">
        <div class="bg-slate-900 rounded-3xl border border-slate-800 overflow-hidden shadow-2xl">
          <table class="w-full text-left">
            <thead class="bg-slate-800/50">
            <tr style="color: #475569; font-size: 10px; font-weight: 900; text-transform: uppercase;">
              <th class="px-6 py-4">Description</th>
              <th class="px-6 py-4">Ticker</th>
              <th class="px-6 py-4 text-right">NAV</th>
              <th class="px-6 py-4 text-center">Action</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="fund in funds" :key="fund.id" class="border-t border-slate-800 hover:bg-emerald-500/5 transition-colors">
              <td class="px-6 py-5 font-bold" style="color: #f1f5f9;">{{ fund.name }}</td>
              <td class="px-6 py-5">
                <span class="bg-slate-950 px-2 py-1 rounded border border-emerald-500/30 text-[10px]" style="color: #10b981;">{{ fund.ticker }}</span>
              </td>
              <td class="px-6 py-5 text-right font-mono font-bold" style="color: #10b981; font-size: 18px;">₹{{ fund.nav.toLocaleString() }}</td>
              <td class="px-6 py-5 text-center">
                <button @click="deleteFund(fund.id)" style="color: #ef4444;"><Trash2 :size="18"/></button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>

        <div v-if="funds.length > 0" class="bg-slate-900 p-8 rounded-3xl border border-slate-800 flex flex-col items-center">
          <h3 class="mb-6 font-bold uppercase tracking-widest text-[10px]" style="color: #94a3b8;">Capital Allocation</h3>
          <div class="w-64 h-64">
            <Pie :data="chartData" />
          </div>
        </div>
      </section>
    </main>
  </div>
</template>